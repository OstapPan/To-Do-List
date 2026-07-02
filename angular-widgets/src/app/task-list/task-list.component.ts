import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskService } from '../services/task.service';
import { Task } from '../models/task.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  @Input() refreshTrigger: number = 0;
  tasks: Task[] = [];
  isLoading = false;
  errorMessage = '';

  @Output() taskDeleted = new EventEmitter<number>();
  @Output() taskEdit = new EventEmitter<Task>(); // Подія для едіту

  constructor(private taskService: TaskService) {}

  ngOnInit() { this.loadTasks(); }

  ngOnChanges() { if (this.refreshTrigger > 0) this.loadTasks(); }

 

  onEdit(task: Task) {
    this.taskEdit.emit(task); // Передаємо задачу в батьківський компонент
  }

  onDelete(id: number) {
    if (!confirm('Delete this task?')) return;
    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.tasks = this.tasks.filter(t => t.id !== id);
        this.taskDeleted.emit(id);
      },
      error: () => this.errorMessage = 'Failed to delete task'
    });
  }
  deleteId: number | null = null; // ID завдання, яке очікує підтвердження

onConfirmDelete(id: number) {
  this.taskService.deleteTask(id).subscribe(() => {
    this.tasks = this.tasks.filter(t => t.id !== id);
    this.deleteId = null;
  });
}
currentPage = 1;
  pageSize = 10;
  totalTasks = 0;

  loadTasks() {
    this.isLoading = true;
    this.taskService.getTasks(this.currentPage, this.pageSize).subscribe({
      next: (response) => { 
        this.tasks = response.items; 
        this.totalTasks = response.totalCount;
        this.isLoading = false; 
      },
      error: () => { this.errorMessage = 'Failed to load tasks'; this.isLoading = false; }
    });
  }

  nextPage() {
    if (this.currentPage * this.pageSize < this.totalTasks) {
      this.currentPage++;
      this.loadTasks();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadTasks();
    }
  }
}
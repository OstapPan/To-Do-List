import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskFormComponent } from '../../task-form/task-form.component'; // Вкажіть правильний шлях
import { TaskListComponent } from '../../task-list/task-list.component'; // Вкажіть правильний шлях
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-tasks-page',
  standalone: true,
  imports: [CommonModule, TaskFormComponent, TaskListComponent],
  template: `
    <div class="page-container">
      
      <app-task-form 
        [taskToEdit]="taskToEdit" 
        (taskSaved)="onTaskSaved()" 
        (cancelEdit)="onCancelEdit()">
      </app-task-form>

      <app-task-list 
        [refreshTrigger]="refreshTrigger" 
        (taskEdit)="onEditTask($event)">
      </app-task-list>
      
    </div>
  `
})
export class TasksPage {
  refreshTrigger = 0;
  taskToEdit: Task | null = null;

  // 1. Користувач натиснув "Edit" у списку
  onEditTask(task: Task) {
    this.taskToEdit = task; // Передаємо дані таски у форму
    window.scrollTo({ top: 0, behavior: 'smooth' }); // Опціонально: скролимо сторінку вгору до форми
  }

  // 2. Форма успішно зберегла (створила або оновила) таску
  onTaskSaved() {
    this.taskToEdit = null; // Повертаємо форму в початковий стан (створення)
    this.refreshTrigger++; // Оновлюємо список
  }

  // 3. Користувач натиснув "Скасувати" у формі
  onCancelEdit() {
    this.taskToEdit = null; // Повертаємо форму в початковий стан
  }
}
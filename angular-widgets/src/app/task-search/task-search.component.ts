import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Необхідно для роботи з [(ngModel)]
import { TaskService } from '../services/task.service';
import { Task } from '../models/task.model';

// Припускаємо, що інтерфейс категорії виглядає так, відповідно до вашої моделі
interface Category {
  id: number;
  name: string;
}

@Component({
  selector: 'app-task-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-search.component.html',
  styleUrls: ['./task-search.component.css']
})
export class TaskSearchComponent implements OnInit {
  categories: Category[] = [];          // Список усіх категорій для селекту
  selectedCategoryId: number | string = ''; // ID вибраної категорії
  tasks: Task[] = [];                    // Список знайдених тасок (task list)
  isLoading = false;
  errorMessage = '';

  constructor(private taskService: TaskService) {}

  ngOnInit() {
    this.loadCategories();
  }

  // Метод для завантаження категорій (адаптуйте під ваш сервіс, якщо є окремий CategoryService)
   loadCategories() {
    
    // Жорстко передаємо page = 1, pageSize = 16 для селекту форми
    this.taskService.getCategories(1, 16).subscribe({
      next: (response) => (this.categories = response.items),
      error: (err) => console.error('Failed to load categories', err)
    });
  
  }
  // Спрацьовує при зміні категорії в селекті
  onCategoryChange() {
    if (!this.selectedCategoryId) {
      this.tasks = [];
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.taskService.getTasksByCategory(Number(this.selectedCategoryId)).subscribe({
      next: (response) => {
        // Записуємо отримані таски в масив (підтримка об'єкта з items або просто масиву)[cite: 2]
        this.tasks = response.items || response; 
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Не вдалося завантажити завдання для цієї категорії';
        this.isLoading = false;
      }
    });
  }
}
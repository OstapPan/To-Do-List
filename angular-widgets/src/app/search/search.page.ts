import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { TaskService } from '../services/task.service';
import { Task } from '../models/task.model';

@Component({
  selector: 'app-task-text-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search.page.html',
  styleUrls: ['./search.page.css'] 
})
export class TaskTextSearchComponent {
  searchText: string = '';               // Текст для пошуку
  tasks: Task[] = [];                    // Список знайдених тасок
  isLoading = false;
  errorMessage = '';
  hasSearched = false;                   // Прапорець для відображення стану "Нічого не знайдено"

  constructor(private taskService: TaskService) {}

  // Спрацьовує при натисканні на кнопку або Enter
  onSearch() {
    const query = this.searchText.trim();
    if (!query) {
      this.tasks = [];
      this.hasSearched = false;
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.hasSearched = true;

    this.taskService.searchTasksByText(query).subscribe({
      next: (response: any) => {
        // Підтримка об'єкта з items або масиву
        this.tasks = response.items || response; 
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Не вдалося виконати пошук за цим текстом';
        this.isLoading = false;
      }
    });
  }
}

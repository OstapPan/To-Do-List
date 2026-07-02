import { Component, OnInit, Output, EventEmitter, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TaskService } from '../services/task.service';
import { Category, Task, TaskCreateRequest, TaskUpdateRequest } from '../models/task.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit, OnChanges {
  @Input() taskToEdit: Task | null = null;
  @Output() taskSaved = new EventEmitter<void>();
  @Output() cancelEdit = new EventEmitter<void>();

  taskForm: FormGroup;
  categories: Category[] = [];
  isSubmitting = false; 

successMessage: string = '';
  errorMessage: string = '';
  constructor(private fb: FormBuilder, private taskService: TaskService) {
    this.taskForm = this.fb.group({
      toDo: ['', Validators.required],
      description: [''],
      doTillDate: ['', Validators.required],
      idCategories: [null, Validators.required],
      isRepetitive: [false],
      howOften: [1]
    });
  }

  ngOnInit() {
    this.loadCategories();
  }

  // Відстежуємо зміни вхідних даних для заповнення форми при кліку на "Edit"
  ngOnChanges(changes: SimpleChanges) {
    if (changes['taskToEdit'] && this.taskToEdit) {
      this.taskForm.patchValue({
        toDo: this.taskToEdit.toDo,
        description: this.taskToEdit.description,
        doTillDate: this.formatDate(this.taskToEdit.doTillDate),
        idCategories: this.taskToEdit.category?.id,
        isRepetitive: this.taskToEdit.isRepetitive,
        howOften: this.taskToEdit.howOften
      });
    } else if (changes['taskToEdit'] && !this.taskToEdit) {
      this.taskForm.reset({ isRepetitive: false, howOften: 1 });
    }
  }

  // Допоміжна функція для форматування дати під формат datetime-local
  private formatDate(date: any) {
    const d = new Date(date);
    return d.toISOString().slice(0, 16);
  }

  loadCategories() {
    
    // Жорстко передаємо page = 1, pageSize = 16 для селекту форми
    this.taskService.getCategories(1, 16).subscribe({
      next: (response) => (this.categories = response.items),
      error: (err) => console.error('Failed to load categories', err)
    });
  
  }

  onCancel() {
    this.taskForm.reset({ isRepetitive: false, howOften: 1 });
    this.cancelEdit.emit();
  }

 onSubmit() {
    if (this.taskForm.invalid) return;

    this.isSubmitting = true;
    // Очищаємо повідомлення перед новим запитом
    this.errorMessage = '';
    this.successMessage = '';

    const payload = { ...this.taskForm.value };

    if (this.taskToEdit) {
      // Режим редагування
      this.taskService.updateTask(this.taskToEdit.id, payload).subscribe({
        next: () => this.handleSuccess('Завдання успішно оновлено!'),
        error: (err) => {
          this.isSubmitting = false;
          this.errorMessage = 'Помилка при оновленні завдання.';
          console.error(err);
        }
      });
    } else {
      // Режим створення
      this.taskService.createTask(payload).subscribe({
        next: () => this.handleSuccess('Завдання успішно створено!'),
        error: (err) => {
          this.isSubmitting = false;
          this.errorMessage = 'Помилка при створенні завдання.';
          console.error(err);
        }
      });
    }
  }

  // Додаємо параметр message для передачі тексту успішної операції
  private handleSuccess(message: string) {
    this.isSubmitting = false;
    this.successMessage = message;
    this.taskForm.reset({ isRepetitive: false, howOften: 1 });
    this.taskSaved.emit();
    
    // За бажанням: можна приховати повідомлення про успіх через 3 секунди
    setTimeout(() => {
      this.successMessage = '';
    }, 3000);
  }
}
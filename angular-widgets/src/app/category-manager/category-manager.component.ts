import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskService } from '../services/task.service';
import { Category } from '../models/task.model';

@Component({
  selector: 'app-category-manager',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './category-manager.component.html',
  styleUrl: './category-manager.component.css'
})
export class CategoryManagerComponent implements OnInit {
  @Output() categoryCreated = new EventEmitter<Category>();

  categories: Category[] = [];
  categoryForm: FormGroup;
  errorMessage = '';
  successMessage = '';
  isLoading = false;
  isSubmitting = false;

  // Стан для модального вікна видалення
  showDeleteModal = false;
  categoryIdToDelete: number | null = null;

  constructor(private taskService: TaskService, private fb: FormBuilder) {
    this.categoryForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  ngOnInit() {
    this.loadCategories();
  }

 currentPage = 1;
  pageSize = 16;
  totalCategories = 0;

  loadCategories() {
    this.isLoading = true;
    this.taskService.getCategories(this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.categories = response.items;
        this.totalCategories = response.totalCount;
        this.isLoading = false;
      },
      error: err => {
        this.errorMessage = 'Failed to load categories';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  nextPage() {
    if (this.currentPage * this.pageSize < this.totalCategories) {
      this.currentPage++;
      this.loadCategories();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadCategories();
    }
  }

  onAdd() {
    if (this.categoryForm.invalid) return;

    this.isSubmitting = true;
    this.successMessage = '';
    this.errorMessage = '';
    const name = this.categoryForm.value.name;

    this.taskService.createCategory(name).subscribe({
      next: category => {
        this.categories.unshift(category);
        this.categoryForm.reset();
        this.successMessage = 'Category created successfully!';
        this.categoryCreated.emit(category);
        this.isSubmitting = false;
        setTimeout(() => (this.successMessage = ''), 3000);
      },
      error: err => {
        this.errorMessage = 'Failed to create category';
        this.isSubmitting = false;
        console.error(err);
      }
    });
  }

  // Клік по кнопці "Видалити" тепер просто відкриває модалку
  onDelete(id: number) {
    this.categoryIdToDelete = id;
    this.showDeleteModal = true;
  }

  // Справжнє видалення після підтвердження в модалці
  confirmDelete() {
    if (this.categoryIdToDelete === null) return;

    const id = this.categoryIdToDelete;
    this.taskService.deleteCategory(id).subscribe({
      next: () => {
        this.categories = this.categories.filter(c => c.id !== id);
        this.successMessage = 'Category deleted successfully!';
        this.closeDeleteModal();
        setTimeout(() => (this.successMessage = ''), 3000);
      },
      error: err => {
        this.errorMessage = 'Failed to delete category';
        this.closeDeleteModal();
        console.error(err);
      }
    });
  }

  // Закриття модального вікна й очищення ID
  closeDeleteModal() {
    this.showDeleteModal = false;
    this.categoryIdToDelete = null;
  }
}
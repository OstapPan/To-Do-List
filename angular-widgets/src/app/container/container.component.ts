import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskFormComponent } from '../task-form/task-form.component';
import { TaskListComponent } from '../task-list/task-list.component';
import { CategoryManagerComponent } from '../category-manager/category-manager.component';

@Component({
  selector: 'app-container',
  standalone: true,
  imports: [CommonModule, TaskFormComponent, TaskListComponent, CategoryManagerComponent],
  template: `
    <div class="space-y-6">
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div class="lg:col-span-1">
          <app-task-form></app-task-form>
        </div>
        <div class="lg:col-span-2">
          <app-task-list></app-task-list>
        </div>
      </div>

      <div>
        <app-category-manager></app-category-manager>
      </div>
    </div>
  `
})
export class ContainerComponent {}

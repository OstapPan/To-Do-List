import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoryManagerComponent } from '../../category-manager/category-manager.component';

@Component({
  selector: 'app-categories-page',
  standalone: true,
  imports: [CommonModule, CategoryManagerComponent],
  templateUrl: './categories.page.html',
  styleUrl: './categories.page.css' // Якщо Angular 17+, інакше: styleUrls: ['./categories-page.component.css']
})
export class CategoriesPage {}
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="home-container">
      <div class="home-hero">
        <h1 class="hero-title">Welcome to To-Do List</h1>
        <p class="hero-subtitle">Manage your tasks and categories efficiently</p>
      </div>

      <div class="home-grid">
        <div class="home-card card-blue">
          <div class="card-header">
            <div>
              <h2 class="card-title">📝 Tasks</h2>
              <p class="card-subtitle">Create and manage your daily tasks</p>
            </div>
            <span class="card-icon">✓</span>
          </div>
          <p class="card-desc">
            Add new tasks, set deadlines, mark as complete, and organize your work efficiently.
          </p>
          <a routerLink="/tasks" class="card-btn btn-blue">
            View Tasks →
          </a>
        </div>

        <div class="home-card card-green">
          <div class="card-header">
            <div>
              <h2 class="card-title">🏷️ Categories</h2>
              <p class="card-subtitle">Organize tasks by category</p>
            </div>
            <span class="card-icon">📂</span>
          </div>
          <p class="card-desc">
            Create custom categories, organize your tasks better, and keep everything structured.
          </p>
          <a routerLink="/categories" class="card-btn btn-green">
            View Categories →
          </a>
        </div>
      </div>

      <div class="features-section">
        <h3 class="features-title">Key Features</h3>
        <div class="features-grid">
          <div class="feature-item">
            <div class="feature-icon">⚡</div>
            <div>
              <h4 class="feature-name">Quick Add</h4>
              <p class="feature-desc">Add tasks instantly with minimal effort</p>
            </div>
          </div>
          <div class="feature-item">
            <div class="feature-icon">📋</div>
            <div>
              <h4 class="feature-name">Organize</h4>
              <p class="feature-desc">Sort tasks by categories and priority</p>
            </div>
          </div>
          <div class="feature-item">
            <div class="feature-icon">🔄</div>
            <div>
              <h4 class="feature-name">Recurring</h4>
              <p class="feature-desc">Set recurring tasks for regular activities</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class HomePage {}
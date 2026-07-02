import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-privacy-page',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="privacy-container">
      <h1 class="privacy-title">🔒 Privacy Policy</h1>
      
      <div class="privacy-content">
        <section class="privacy-section">
          <h2>Introduction</h2>
          <p>
            This privacy policy explains how our To-Do List application collects, uses, and protects your personal information.
          </p>
        </section>

        <section class="privacy-section">
          <h2>Information We Collect</h2>
          <ul class="privacy-list">
            <li>User account information (email, username)</li>
            <li>Task and category data you create</li>
            <li>Usage analytics and error logs</li>
          </ul>
        </section>

        <section class="privacy-section">
          <h2>How We Use Your Information</h2>
          <ul class="privacy-list">
            <li>To provide and improve our service</li>
            <li>To store and manage your tasks</li>
            <li>To send service-related communications</li>
          </ul>
        </section>

        <section class="privacy-section">
          <h2>Data Protection</h2>
          <p>
            We implement appropriate security measures to protect your data from unauthorized access. 
            Your data is stored securely and is never shared with third parties without your consent.
          </p>
        </section>

        <section class="privacy-section">
          <h2>Contact Us</h2>
          <p>
            If you have any questions about our privacy practices, please contact us at <a class="privacy-email" href="mailto:support@todolist.app">support@todolist.app</a>
          </p>
        </section>
      </div>
    </div>
  `
})
export class PrivacyPage {}
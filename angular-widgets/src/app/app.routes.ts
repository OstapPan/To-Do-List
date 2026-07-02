import { Routes } from '@angular/router';
import { HomePage } from './pages/home/home.page';
import { TasksPage } from './pages/tasks/tasks.page';
import { CategoriesPage } from './pages/categories/categories.page';
import { PrivacyPage } from './pages/privacy/privacy.page';
import { RegisterPage } from './pages/privacy/register.page';
import { LoginPage } from './pages/privacy/login.page';
import { TaskSearchComponent } from './task-search/task-search.component';
import { TaskTextSearchComponent } from './search/search.page';

export const routes: Routes = [
  { path: '', component: HomePage },
  { path: 'tasks', component: TasksPage },
  { path: 'categories', component: CategoriesPage },
  { path: 'privacy', component: PrivacyPage },
  { path: 'register', component: RegisterPage },
  { path: 'login', component: LoginPage },
  { path: 'filter', component: TaskSearchComponent },
   { path: 'search', component: TaskTextSearchComponent },
  { path: '**', redirectTo: '' }
];

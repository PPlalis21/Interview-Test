import { Component, OnInit, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { UserListItem } from '../../../models/user.model';

// หน้า users list + search
@Component({
  standalone: true,
  selector: 'app-users-list',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './users-list.component.html',
})
export class UsersListComponent implements OnInit {
  // state (signals)
  users = signal<UserListItem[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  search = signal('');

  // filter ตามคำค้น
  filtered = computed(() => {
    const term = this.search().trim().toLowerCase();
    if (!term) return this.users();
    return this.users().filter(u =>
      u.id.toLowerCase().includes(term) ||
      u.userId.toLowerCase().includes(term) ||
      u.username.toLowerCase().includes(term) ||
      `${u.firstName} ${u.lastName}`.toLowerCase().includes(term)
    );
  });

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loading.set(true);
    this.userService.getUsers().subscribe({
      next: data => {
        this.users.set(data ?? []);
        this.loading.set(false);
      },
      error: err => {
        console.error(err);
        this.error.set('Failed to load users');
        this.loading.set(false);
      }
    });
  }

  onSearch(value: string) {
    this.search.set(value);
  }
}

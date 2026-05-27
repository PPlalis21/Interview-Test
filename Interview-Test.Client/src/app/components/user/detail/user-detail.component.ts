import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { UserDetail } from '../../../models/user.model';

// หน้า user detail (อ่าน id จาก URL)
@Component({
  standalone: true,
  selector: 'app-user-detail',
  imports: [CommonModule, RouterLink],
  templateUrl: './user-detail.component.html',
})
export class UserDetailComponent implements OnInit {
  user = signal<UserDetail | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  constructor(private route: ActivatedRoute, private userService: UserService) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error.set('Missing user id');
      return;
    }
    this.loading.set(true);
    this.userService.getUserById(id).subscribe({
      next: data => {
        this.user.set(data);
        this.loading.set(false);
      },
      error: err => {
        console.error(err);
        this.error.set('Failed to load user');
        this.loading.set(false);
      }
    });
  }
}

import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { RoleService } from '../../../services/role.service';
import { CreateUserPayload, UserRole } from '../../../models/user.model';

// หน้าฟอร์มสร้าง user → POST /CreateUser
@Component({
  standalone: true,
  selector: 'app-user-create',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './user-create.component.html',
})
export class UserCreateComponent implements OnInit {
  // role master จาก API
  roleOptions = signal<UserRole[]>([]);
  rolesLoading = signal(false);
  rolesError = signal<string | null>(null);

  // form state
  userId = signal('');
  username = signal('');
  firstName = signal('');
  lastName = signal('');
  age = signal<number | null>(null);
  selectedRoleIds = signal<Set<number>>(new Set());

  submitting = signal(false);
  error = signal<string | null>(null);

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private router: Router
  ) {}

  // โหลด role master
  ngOnInit(): void {
    this.rolesLoading.set(true);
    this.roleService.getRoles().subscribe({
      next: data => {
        this.roleOptions.set(data ?? []);
        this.rolesLoading.set(false);
      },
      error: err => {
        console.error(err);
        this.rolesError.set('โหลด role ไม่สำเร็จ');
        this.rolesLoading.set(false);
      }
    });
  }

  // toggle checkbox (Set ใหม่ทุกครั้งให้ signal detect ได้)
  toggleRole(roleId: number, checked: boolean) {
    const next = new Set(this.selectedRoleIds());
    checked ? next.add(roleId) : next.delete(roleId);
    this.selectedRoleIds.set(next);
  }

  isRoleSelected(roleId: number): boolean {
    return this.selectedRoleIds().has(roleId);
  }

  submit() {
    // validate
    if (!this.userId().trim() || !this.username().trim()
        || !this.firstName().trim() || !this.lastName().trim()) {
      this.error.set('กรุณากรอกข้อมูลให้ครบ (UserId, Username, FirstName, LastName)');
      return;
    }
    if (this.selectedRoleIds().size === 0) {
      this.error.set('กรุณาเลือกอย่างน้อย 1 role');
      return;
    }

    // build payload
    const payload: CreateUserPayload = {
      userId: this.userId().trim(),
      username: this.username().trim(),
      firstName: this.firstName().trim(),
      lastName: this.lastName().trim(),
      age: this.age(),
      userRoles: Array.from(this.selectedRoleIds()).map(roleId => ({ role: { roleId } }))
    };

    // submit
    this.error.set(null);
    this.submitting.set(true);
    this.userService.createUser(payload).subscribe({
      next: res => {
        // backend คืน 200 พร้อม success=false ถ้า UserId ซ้ำ → แสดง message จาก backend
        if (res.success) {
          this.router.navigate(['/users']);
        } else {
          this.error.set(res.message || 'สร้าง user ไม่สำเร็จ');
          this.submitting.set(false);
        }
      },
      error: err => {
        console.error(err);
        this.error.set('สร้าง user ไม่สำเร็จ (เกิดข้อผิดพลาด)');
        this.submitting.set(false);
      }
    });
  }
}

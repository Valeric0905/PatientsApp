import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]  // Ensure necessary modules are imported
})
export class RegisterComponent {
  username: string = '';
  password: string = '';
  role: string = 'patient';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  register() {
    if (this.username && this.password) {
      this.authService.register(this.username, this.password, this.role).subscribe(() => {
        this.router.navigate(['/login']);
      });
    } else {
      this.errorMessage = 'Please enter valid details';
    }
  }
}




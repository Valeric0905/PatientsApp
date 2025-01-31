import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]  // Import FormsModule here
})
export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor() { }

  login() {
    console.log('Button clicked');
    if (this.username && this.password) {
      // Authentication logic
    } else {
      console.log('Please enter username and password');
    }
  }
}


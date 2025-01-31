import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const user = this.authService.currentUserValue;
    const expectedRole = next.data['role'];

    if (!user) {
      this.router.navigate(['/login']);
      return false;
    }

    if (expectedRole && user.role !== expectedRole) {
      this.router.navigate(['/dashboard']);
      return false;
    }

    return true;
  }
}

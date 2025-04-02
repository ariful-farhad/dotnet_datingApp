import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { catchError } from 'rxjs';
import { CommonModule, NgIf } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, CommonModule, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  accountService = inject(AccountService);

  model: any = {};

  login() {
    // this.model = { username: 'hello', password: #loginForm.password };
    // console.log(this.model);
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => console.log(error),
      complete() {
        console.log('request has been complete');
      },
    });
  }
  logout() {
    this.accountService.logout();
  }
}

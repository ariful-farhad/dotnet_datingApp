import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  imports: [],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.css',
})
export class ServerErrorComponent {
  error: any;

  // why this constructor?
  constructor(private router: Router) {
    // ##########################this is the code we wrote in the interceptor#########################################################
    //
    // // case 500:
    //   // we want to send the status as well of the error, to show something when we navigate to the page
    //   const navigationExtras: NavigationExtras = {
    //     state: { error: error.error },
    //   };
    //   router.navigateByUrl('/server-error', navigationExtras);
    //   break;

    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.['error'];
  }
}

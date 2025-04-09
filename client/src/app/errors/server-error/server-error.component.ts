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
/*
Okay, let's break down why this `constructor` is set up the way it is, connecting it directly to your `errorInterceptor`.

1.  **What is a Constructor?**
    * In object-oriented programming (which TypeScript and Angular use), a `constructor` is a special function that runs **automatically** and **only once** when a new instance (a copy) of a class (like your `ServerErrorComponent`) is created.
    * Its main jobs are:
        * **Initialization:** Setting up the initial state of the object.
        * **Dependency Injection (in Angular):** Telling Angular what services this component needs to function.

2.  **Dependency Injection: `constructor(private router: Router)`**
    * This part tells Angular: "Hey, when you create an instance of `ServerErrorComponent`, I need you to also give me an instance of the `Router` service."
    * The `Router` service is Angular's built-in tool for handling navigation between different pages or views in your application.
    * `private router:` makes the injected `Router` instance available as a property named `router` within this component, so you can use it (like `this.router...`).
    * **Why need the Router here?** Because you need to get information about *how the user arrived* at this specific `/server-error` page.

3.  **Connecting to the Interceptor's Action (`case 500`)**
    * Remember, in your `errorInterceptor`, when a 500 error happens:
        * You create `navigationExtras` containing `state: { error: error.error }`. This bundles the actual error details received from the server into a temporary package.
        * You call `router.navigateByUrl('/server-error', navigationExtras);`. This tells the `Router` to navigate the user to the `/server-error` route **AND** to carry that `navigationExtras` package along with the navigation *in memory*. This data is **not** put into the URL itself.

4.  **Getting the Data Passed During Navigation: The Constructor's Job**
    * Now, the user lands on the `/server-error` route, and Angular creates an instance of your `ServerErrorComponent`. The constructor runs immediately.
    * **`const navigation = this.router.getCurrentNavigation();`**: Inside the constructor, you use the injected `router` to ask: "Tell me about the navigation event that just brought the user here." This gives you access to information about that specific navigation, including any `extras` that were sent. This `getCurrentNavigation()` method typically only returns useful information *during* the navigation lifecycle (which includes the constructor and `ngOnInit`).
    * **`this.error = navigation?.extras?.state?.['error'];`**: This is the crucial line for retrieving the data:
        * `navigation?`: Safely checks if `navigation` data exists.
        * `.extras?`: Safely looks inside for the `extras` object (the `navigationExtras` package you sent).
        * `.state?`: Safely looks inside `extras` for the `state` object (the `{ error: ... }` part you defined).
        * `?.['error']`: Safely tries to access the property named `error` *within* that `state` object.
        * The value found (which is the `error.error` object originally passed by the interceptor) is then assigned to the `ServerErrorComponent`'s own `error` property.

**Why do this in the `constructor`?**

The `constructor` (along with `ngOnInit`, another lifecycle hook) is the **earliest reliable place** to access the `state` data passed during navigation using `getCurrentNavigation()`. You need to capture this "in-flight" data as the component is being initialized.

  
*/

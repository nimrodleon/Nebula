**Edit a file, create a new file, and clone from Bitbucket in under 2 minutes**

When you're done, you can delete the content in this README and update the file with details for others getting started
with your repository.

*We recommend that you open this README in another tab as you perform the tasks below. You
can [watch our video](https://youtu.be/0ocf7u76WSo) for a full demo of all the steps in this tutorial. Open the video in
a new tab to avoid leaving Bitbucket.*

---

## Paquetes del proyecto.

- [jdenticon](https://github.com/dmester/jdenticon) (library for generating identicons).

## Usage Font Awesome

To get up and running using Font Awesome with Angular follow below steps:

1. Add `FontAwesomeModule` to `imports` in
   `src/app/app.module.ts`:

    ```typescript
    import { BrowserModule } from '@angular/platform-browser';
    import { NgModule } from '@angular/core';
    
    import { AppComponent } from './app.component';
    import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
    
    @NgModule({
      imports: [
        BrowserModule,
        FontAwesomeModule
      ],
      declarations: [AppComponent],
      bootstrap: [AppComponent]
    })
    export class AppModule { }
    ```

2. Tie the icon to the property in your component
   `src/app/app.component.ts`:

    ```typescript
    import { Component } from '@angular/core';
    import { faCoffee } from '@fortawesome/free-solid-svg-icons';
    
    @Component({
      selector: 'app-root',
      templateUrl: './app.component.html'
    })
    export class AppComponent {
      faCoffee = faCoffee;
    }
    ```

3. Use the icon in the template
   `src/app/app.component.html`:

    ```html
    <fa-icon [icon]="faCoffee"></fa-icon>
    ```

## Consulta Órdenes de reparación.

```
db.contacts.aggregate([
    {
        "$lookup": {
            "from": "order_repairs", "localField": "_id", "foreignField": "client_id", "as": "OrderRepair"
        }
    },
    {
        "$project": {
            "OrderRepair": {
                "$filter": {
                    "input": "$OrderRepair",
                    "as": "rep",
                    "cond": {
                        "$eq": ["$$rep.is_deleted", false]
                    }
                }
            }
        }
    },
    {
        "$unwind": "$OrderRepair"
    }
])
```

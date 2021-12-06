import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {ResponseData} from 'src/app/global/interfaces';
import {PeopleDocTypeService} from '../../services';
import {PeopleDocType} from '../../interfaces';

@Component({
  selector: 'app-doc-type-modal',
  templateUrl: './doc-type-modal.component.html',
  styleUrls: ['./doc-type-modal.component.scss']
})
export class DocTypeModalComponent implements OnInit {
  faBars = faBars;
  @Input()
  title: string = '';
  @Input()
  docType: PeopleDocType | any;
  @Output()
  responseData = new EventEmitter<ResponseData<PeopleDocType>>();
  docTypeForm: FormGroup = this.fb.group({
    id: [null],
    description: [''],
    sunatCode: ['']
  });

  constructor(
    private fb: FormBuilder,
    private peopleDocTypeService: PeopleDocTypeService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#doc-type-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      if (this.docType !== null) {
        this.docTypeForm.reset(this.docType);
      }
    });
    myModal.addEventListener('hide.bs.modal', () => {
      this.docTypeForm.addControl('id', new FormControl(null));
      this.docTypeForm.reset();
    });
  }

  // guardar cambios.
  public saveChanges(): void {
    if (this.docTypeForm.get('id')?.value === null) {
      this.docTypeForm.removeControl('id');
      this.peopleDocTypeService.create(this.docTypeForm.value)
        .subscribe(result => this.responseData.emit(result));
    } else {
      const id = this.docTypeForm.get('id')?.value;
      this.peopleDocTypeService.update(id, this.docTypeForm.value)
        .subscribe(result => this.responseData.emit(result));
    }
  }

}

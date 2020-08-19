import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MarketDataService } from 'src/app/Shared/Services/market-data.service';
import { Ng2IzitoastService } from 'ng2-izitoast';
import { MarketDataComponentNotifierService } from 'src/app/Shared/Services/market-data-component-notifier.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {

  @ViewChild('marketFileInput', { static: false }) InputVar: ElementRef;

  private file: any;
  fileUploadText: string = "Choose a file";


  constructor(private service: MarketDataService,
    private iziToast: Ng2IzitoastService,
    private notifier: MarketDataComponentNotifierService) {

  }

  ngOnInit() {

  }

  fileChanged(e) {
    this.file = e.target.files[0];
    this.fileUploadText = this.file.name;
  }

  sendFile() {
    if (!this.file) {
      this.iziToast.warning({ message: 'Please select a file.' });
      return;
    }
    this.iziToast.info({ message: 'Uploading...' });
    let fileReader = new FileReader();
    fileReader.onload = (e) => {
      let lines = fileReader.result.toString().split('\r\n');
      this.service.InsertRecords(lines)
        .subscribe((result) => {
          this.resetFileInput();
          this.notifier.updateMessage();
          this.iziToast.success({ message: 'File successfully imported!' });
          this.iziToast.success({
            title: 'Results:', message: `
          Valid Records: ${result.validRecords.length}
          Malformed Records: ${result.invalidRecords.length}
          New Records: ${result.newRecords.length}
          `, timeout: 0
          });
        });
    }
    fileReader.readAsText(this.file);
  }

  resetFileInput() {
    this.file = null;
    this.InputVar.nativeElement.value = "";
    this.fileUploadText = "Choose a file";
  }
}

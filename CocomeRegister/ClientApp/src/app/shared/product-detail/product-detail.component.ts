import { HttpClient } from '@angular/common/http';
import { EventEmitter, Inject } from '@angular/core';
import { Component, Input, Output } from '@angular/core';
import { Product, Provider } from 'src/services/Models';

export interface Resource {
  path: string;
}

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent {
  @Input() product: Product = {} as Product;
  @Input() providers: Provider[];
  @Input() restricted: boolean;
  @Output() updateProductEvent = new EventEmitter<Product>();

  baseUrl: string

  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  updateImage(files: FileList) {
    if (files.length > 0) {
      const file = files[0];
      const data = new FormData();
      data.append("file", file);
      this.http.post<Resource>(`${this.baseUrl}api/fileupload`, data).subscribe(result => {
        this.product.imageUrl = result.path;
      }, error => console.error(error));
    }
  }

  updateProduct() {
    this.updateProductEvent.emit(this.product);
  }
}

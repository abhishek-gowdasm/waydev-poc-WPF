import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { Subject, takeUntil } from 'rxjs';
import { ApiService } from '../services/api.service';
import { ProductDto, CreateProductDto, UpdateProductDto } from '../models/dashboard.model';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatCardModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatCheckboxModule
  ],
  templateUrl: './products.html',
  styleUrls: ['./products.scss']
})
export class ProductsComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  products: ProductDto[] = [];
  dataSource = new MatTableDataSource<ProductDto>();
  loading = false;
  showForm = false;
  editingProduct: ProductDto | null = null;
  productForm: FormGroup;

  displayedColumns: string[] = ['productID', 'productName', 'categoryName', 'unitPrice', 'unitsInStock', 'discontinued', 'actions'];

  private destroy$ = new Subject<void>();

  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) {
    this.productForm = this.fb.group({
      ProductName: ['', [Validators.required, Validators.minLength(2)]],
      SupplierID: [null, [Validators.required, Validators.min(1)]],
      CategoryID: [null, [Validators.required, Validators.min(1)]],
      QuantityPerUnit: ['', Validators.required],
      UnitPrice: [null, [Validators.required, Validators.min(0.01)]],
      UnitsInStock: [null, [Validators.min(0)]],
      UnitsOnOrder: [null, [Validators.min(0)]],
      ReorderLevel: [null, [Validators.min(0)]],
      Discontinued: [false]
    });
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadProducts(): void {
    this.loading = true;
    this.apiService.getProducts()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (products) => {
          this.products = products || [];
          this.dataSource.data = this.products;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading products:', error);
          this.products = [];
          this.dataSource.data = [];
          this.loading = false;
          this.snackBar.open('Error loading products', 'Close', { duration: 3000 });
        }
      });
  }

  addProduct(): void {
    this.editingProduct = null;
    this.productForm.reset();
    this.productForm.patchValue({
      Discontinued: false
    });
    this.showForm = true;
  }

  editProduct(product: ProductDto): void {
    this.editingProduct = product;
    this.productForm.patchValue({
      ProductName: product.ProductName,
      SupplierID: product.SupplierID,
      CategoryID: product.CategoryID,
      QuantityPerUnit: product.QuantityPerUnit,
      UnitPrice: product.UnitPrice,
      UnitsInStock: product.UnitsInStock,
      UnitsOnOrder: product.UnitsOnOrder,
      ReorderLevel: product.ReorderLevel,
      Discontinued: product.Discontinued
    });
    this.showForm = true;
  }

  saveProduct(): void {
    if (this.productForm.valid) {
      const productData = this.productForm.value;
      
      if (this.editingProduct) {
        // Update existing product
        const updateData: UpdateProductDto = {
          ProductName: productData.ProductName,
          SupplierID: productData.SupplierID,
          CategoryID: productData.CategoryID,
          QuantityPerUnit: productData.QuantityPerUnit,
          UnitPrice: productData.UnitPrice,
          UnitsInStock: productData.UnitsInStock,
          UnitsOnOrder: productData.UnitsOnOrder,
          ReorderLevel: productData.ReorderLevel,
          Discontinued: productData.Discontinued ?? false
        };
        
        this.loading = true;
        this.apiService.updateProduct(this.editingProduct.ProductID, updateData)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (updatedProduct) => {
              this.snackBar.open('Product updated successfully!', 'Close', { duration: 3000 });
              this.showForm = false;
              this.loadProducts(); // Reload the data
            },
            error: (error) => {
              console.error('Error updating product:', error);
              this.snackBar.open('Error updating product. Please try again.', 'Close', { duration: 3000 });
            },
            complete: () => {
              this.loading = false;
            }
          });
      } else {
        // Add new product
        const newProduct: CreateProductDto = {
          ProductName: productData.ProductName,
          SupplierID: productData.SupplierID,
          CategoryID: productData.CategoryID,
          QuantityPerUnit: productData.QuantityPerUnit,
          UnitPrice: productData.UnitPrice,
          UnitsInStock: productData.UnitsInStock,
          UnitsOnOrder: productData.UnitsOnOrder,
          ReorderLevel: productData.ReorderLevel,
          Discontinued: productData.Discontinued ?? false
        };
        
        console.log('Form Data:', productData);
        console.log('Sending to API:', newProduct);
        
        this.loading = true;
        this.apiService.createProduct(newProduct)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (createdProduct) => {
              console.log('Product created successfully:', createdProduct);
              this.snackBar.open('Product added successfully!', 'Close', { duration: 3000 });
              this.showForm = false;
              this.loadProducts(); // Reload the data
            },
            error: (error) => {
              console.error('Error creating product:', error);
              console.error('Error details:', error.error);
              this.snackBar.open('Error creating product. Please try again.', 'Close', { duration: 3000 });
            },
            complete: () => {
              this.loading = false;
            }
          });
      }
    }
  }

  deleteProduct(product: ProductDto): void {
    if (confirm(`Are you sure you want to delete ${product.ProductName}?`)) {
      this.loading = true;
      this.apiService.deleteProduct(product.ProductID)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.snackBar.open('Product deleted successfully!', 'Close', { duration: 3000 });
            this.loadProducts(); // Reload the data
          },
          error: (error) => {
            console.error('Error deleting product:', error);
            this.snackBar.open('Error deleting product. Please try again.', 'Close', { duration: 3000 });
          },
          complete: () => {
            this.loading = false;
          }
        });
    }
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingProduct = null;
    this.productForm.reset();
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  get totalProducts(): number {
    return this.products.length;
  }

  get totalCategories(): number {
    return new Set(this.products.map(p => p.CategoryName)).size;
  }

  get totalValue(): number {
    return this.products.reduce((sum, product) => {
      const price = product.UnitPrice || 0;
      const stock = product.UnitsInStock || 0;
      return sum + (price * stock);
    }, 0);
  }

  get lowStockProducts(): number {
    return this.products.filter(p => (p.UnitsInStock || 0) < 10).length;
  }
} 
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ProductDto, CreateProductDto, UpdateProductDto } from '../models/dashboard.model';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private productsSubject = new BehaviorSubject<ProductDto[]>([]);
  public products$ = this.productsSubject.asObservable();

  constructor(private apiService: ApiService) {
    this.loadProducts();
  }

  loadProducts(): void {
    this.apiService.getProducts().subscribe({
      next: (products) => {
        if (products && products.length > 0) {
          // Map backend properties to frontend format if needed
          const mappedProducts = products.map((product: any) => ({
            ProductID: product.ProductID || 0,
            ProductName: product.ProductName || '',
            SupplierID: product.SupplierID || null,
            CategoryID: product.CategoryID || null,
            QuantityPerUnit: product.QuantityPerUnit || '',
            UnitPrice: product.UnitPrice || null,
            UnitsInStock: product.UnitsInStock || null,
            UnitsOnOrder: product.UnitsOnOrder || null,
            ReorderLevel: product.ReorderLevel || null,
            Discontinued: product.Discontinued || false,
            CategoryName: product.CategoryName || '',
            SupplierName: product.SupplierName || '',
            TotalSales: product.TotalSales || 0,
            OrderCount: product.OrderCount || 0
          }));
          this.productsSubject.next(mappedProducts);
        }
      },
      error: (error) => {
        console.error('Error loading products:', error);
      }
    });
  }

  getProducts(): ProductDto[] {
    return this.productsSubject.value;
  }

  getProduct(id: number): Observable<ProductDto> {
    return this.apiService.getProduct(id);
  }

  addProduct(productData: CreateProductDto): void {
    this.apiService.createProduct(productData).subscribe({
      next: (newProduct) => {
        const currentProducts = this.productsSubject.value;
        this.productsSubject.next([...currentProducts, newProduct]);
      },
      error: (error) => {
        console.error('Error creating product:', error);
      }
    });
  }

  updateProduct(id: number, productData: UpdateProductDto): void {
    this.apiService.updateProduct(id, productData).subscribe({
      next: (updatedProduct) => {
        const currentProducts = this.productsSubject.value;
        const updatedProducts = currentProducts.map(product =>
          product.ProductID === id ? updatedProduct : product
        );
        this.productsSubject.next(updatedProducts);
      },
      error: (error) => {
        console.error('Error updating product:', error);
      }
    });
  }

  deleteProduct(id: number): void {
    this.apiService.deleteProduct(id).subscribe({
      next: () => {
        const currentProducts = this.productsSubject.value;
        const filteredProducts = currentProducts.filter(product => product.ProductID !== id);
        this.productsSubject.next(filteredProducts);
      },
      error: (error) => {
        console.error('Error deleting product:', error);
      }
    });
  }

  refreshProducts(): void {
    this.loadProducts();
  }

  searchProducts(searchTerm: string): Observable<ProductDto[]> {
    return this.apiService.searchProducts(searchTerm);
  }

  getProductsByCategory(categoryId: number): Observable<ProductDto[]> {
    return this.apiService.getProductsByCategory(categoryId);
  }

  getProductsBySupplier(supplierId: number): Observable<ProductDto[]> {
    return this.apiService.getProductsBySupplier(supplierId);
  }

  getDiscontinuedProducts(): Observable<ProductDto[]> {
    return this.apiService.getDiscontinuedProducts();
  }

  getLowStockProducts(): Observable<ProductDto[]> {
    return this.apiService.getLowStockProducts();
  }

  getTopProducts(count: number): Observable<ProductDto[]> {
    return this.apiService.getTopProductsBySales(count);
  }
} 
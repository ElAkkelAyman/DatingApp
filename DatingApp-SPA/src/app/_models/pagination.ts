export interface Pagination {
    currentPage: number;
    itemPerPage: number;
    totalItems: number;
    toralPgaes: number;
}

export class PaginatedResult<T> {
 result: T;
 pagination: Pagination;   
}

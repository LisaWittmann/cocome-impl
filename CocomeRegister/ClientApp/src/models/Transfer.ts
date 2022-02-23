
export interface Statistic {
    label: string;
    dataset: number[];
}

export interface PagedResponse<T> {
    pageNumber: number;
    pageSize: number;
    firstPage: string | undefined;
    lastPage: string | undefined;
    totalPages: number;
    totalRecords: number;
    nextPage: string | undefined;
    previousPage: string | undefined;
    data: T;
}

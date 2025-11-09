import { MemberDto } from './member-dto';
import { PaginationMetadata } from './PaginationMetadata';
export interface PaginationData<T> {
  data: T[];
  paginationMetadata: PaginationMetadata;
}

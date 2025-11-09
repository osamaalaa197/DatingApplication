import { PhotoDto } from './photo-dto';

export interface MemberDto {
  id: string;
  username: string;
  knownAs: string;
  createdOn: string;
  lastActive: string;
  gender: string;
  introduction: string;
  interests: string;
  lookingFor: string;
  city: string;
  country: string;
  photos: PhotoDto[];
  age: number;
  photoUrl: string;
}

export interface UserInfoDto {
    email: string;
    firstName?: string;
    lastName?: string;
    createdDate?: Date | string;
    organiserName?: string;
    confirmedSeller?: boolean;
}
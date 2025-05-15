import {UserInfoDto} from "../models/dtos/UserInfoDto.ts";
import {apiRequest} from "./httpClient.ts";
import {API_ENDPOINTS, getApiEndpoint} from "../assets/constants/api.ts";


const userService = {
    getUserInfo: async (): Promise<UserInfoDto> => {
        try {
            return await apiRequest(getApiEndpoint(API_ENDPOINTS.USER_INFO));
        } 
        catch (error) {
            if (error instanceof Error) {
                throw new Error(error.message || 'Failed to fetch user info');
            } else {
                throw new Error('Failed to fetch user info');
            }
        }
    }
};

export default userService;
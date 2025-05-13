import {UserInfoDto} from "../models/dtos/UserInfoDto.ts";
import {apiRequest} from "./httpClient.ts";
import {apiEndpoints, getApiEndpoint} from "../assets/constants/api.ts";


const userService = {
    getUserInfo: async (): Promise<UserInfoDto> => {
        try {
            return await apiRequest(getApiEndpoint(apiEndpoints.USER_INFO));
        } catch (error: any) {
            throw new Error(error.message || 'Failed to fetch user info');
        }
    },
};

export default userService;
import {CartInfoResponse} from "../dtos/CartDto.ts";
import {apiRequest} from "./httpClient.ts";
import {API_ENDPOINTS, getApiEndpoint} from "../assets/constants/api.ts";


const cartService = {
    getCartInfo: async (): Promise<CartInfoResponse> => {
        try {
            return await apiRequest(getApiEndpoint(API_ENDPOINTS.CART_INFO));
        } 
        catch (error: any) {
            throw new Error(error.message || 'Failed to fetch cart info');
        }
    }
};

export default cartService;
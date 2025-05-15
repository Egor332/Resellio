import { useEffect, useState } from 'react';
import {UserInfoDto} from "../models/dtos/UserInfoDto.ts";
import userService from "../services/userService.ts";

export const useUserInfo = () => {
    const [userInfo, setUserInfo] = useState<UserInfoDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchUserInfo = async () => {
            try {
                const data = await userService.getUserInfo();
                setUserInfo(data);
            } catch (err: any) {
                setError(err.message || 'Failed to fetch user info');
            } finally {
                setLoading(false);
            }
        };

        fetchUserInfo();
    }, []);

    return { userInfo, loading, error };
};
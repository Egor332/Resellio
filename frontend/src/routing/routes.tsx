import { RouteObject } from "react-router-dom";
import { Navigation } from "../assets/constants/navigation";
import GuestWrapperPage from "../pages/GuestWrapperPage/GuestWrapperPage";
import Login from "../pages/Login/Login";
import RegisterCustomer from "../pages/RegisterCustomer/RegisterCustomer";
import RegisterOrganiser from "../pages/RegisterOrganiser/RegisterOrganiser";

export const routes: RouteObject[] = [
    {
        path: Navigation.ROOT,
        element: <GuestWrapperPage />,
        children: [
            {
                index: true,
                element: <Login />,
            },
            {
                path: Navigation.REGISTER_CUSTOMER,
                element: <RegisterCustomer />,
            },
            {
                path: Navigation.REGISTER_ORGANISER,
                element: <RegisterOrganiser />,
            }
        ]
    }
];
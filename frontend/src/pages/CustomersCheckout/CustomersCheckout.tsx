import {useParams} from "react-router";

const CustomersCheckout = () => {
    const {sellerId} = useParams<{sellerId: string}>()
    return (
        <div>
            Checkout for seller {sellerId}
        </div>
    );
};

export default CustomersCheckout;
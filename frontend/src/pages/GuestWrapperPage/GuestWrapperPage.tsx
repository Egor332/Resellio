import { Outlet } from "react-router-dom";
import GuestHeader from "../../components/GuestHeader/GuestHeader";
import Footer from "../../components/Footer/Footer";
import styles from './GuestWrapperPage.module.css';

function GuestWrapperPage() {
  return (
    <div className={styles['guest-wrapper']}>
      <GuestHeader />
      <div className={styles['content']}>
        <Outlet />
      </div>
      <Footer />
    </div>
  );
};

export default GuestWrapperPage;
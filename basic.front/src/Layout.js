import LayoutMenu from "./LayoutMenu";
import LayoutHeader from "./LayoutHeader";

function Layout({ children }) {
    return (
        <>
            <LayoutMenu />
            <LayoutHeader />
            <div className="page-wrapper">
                {children}
            </div>
        </>
    );
}

export default Layout;

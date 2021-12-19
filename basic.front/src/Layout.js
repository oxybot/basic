import LayoutMenu from "./LayoutMenu";
import LayoutHeader from "./LayoutHeader";

export default function Layout({ children }) {
  return (
    <>
      <LayoutMenu />
      <LayoutHeader />
      <div className="page-wrapper">{children}</div>
    </>
  );
}

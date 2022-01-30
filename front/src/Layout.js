import LayoutMenu from "./LayoutMenu";

export default function Layout({ children }) {
  return (
    <>
      <LayoutMenu />
      <div className="page-wrapper">{children}</div>
    </>
  );
}

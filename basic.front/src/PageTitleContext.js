import React, { useContext, useEffect } from "react";

export const PageTitleContext = React.createContext(["", () => {}]);

export function usePageTitle(newPageTitle = undefined) {
  const [pageTitle, setPageTitle] = useContext(PageTitleContext);
  useEffect(() => {
    if (newPageTitle !== undefined) {
      setPageTitle(newPageTitle);
    }
  }, [setPageTitle, newPageTitle]);

  return pageTitle;
}

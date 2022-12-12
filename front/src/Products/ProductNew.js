import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { useRevalidator } from "react-router-dom";

export function ProductNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("ProductForEdit");
  const texts = {
    title: "Products",
    subTitle: "Add a new Product",
  };

  function handleCreate() {
    revalidator.revalidate();
  }

  return <PageNew definition={definition} baseApiUrl="Products" texts={texts} onCreate={handleCreate} />;
}

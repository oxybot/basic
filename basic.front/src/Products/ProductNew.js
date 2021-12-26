import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function ProductNew() {
  const definition = useDefinition("ProductForEdit");
  const texts = {
    title: "Products",
    subTitle: "Add a new Product",
  };

  return <PageNew definition={definition} baseApiUrl="Products" texts={texts} />;
}

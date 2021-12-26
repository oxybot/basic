import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageList from "../Generic/PageList";

export function ProductList() {
  const { productId } = useParams();
  const definition = useDefinition("ProductForList");
  const [loading, elements] = useApiFetch("Products", { method: "GET" }, []);
  const texts = {
    title: "Products",
    add: "Add product",
  };

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={productId}
      texts={texts}
    />
  );
}

import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { useSorting } from "../helpers/sorting";

export function ProductList() {
  const { productId } = useParams();
  const definition = useDefinition("ProductForList");
  const texts = {
    title: "Products",
    add: "Add product",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={productId}
      texts={texts}
      newRole="client"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}

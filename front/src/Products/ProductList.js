import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { disconnect, productsState, retrieveAll, setSorting } from "./slice";

export function ProductList() {
  const dispatch = useDispatch();
  const { productId } = useParams();
  const definition = useDefinition("ProductForList");
  const texts = {
    title: "Products",
    add: "Add product",
  };

  const { loading, elements, sorting } = useSelector(productsState);

  useEffect(() => {
    dispatch(retrieveAll());
    return () => dispatch(disconnect());
  }, [dispatch, sorting]);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={productId}
      texts={texts}
      newRole="client"
      sorting={sorting}
      setSorting={(s) => dispatch(setSorting(s))}
    />
  );
}

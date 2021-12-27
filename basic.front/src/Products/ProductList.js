import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { disconnect, getAll, productsState } from "./slice";

export function ProductList() {
  const dispatch = useDispatch();
  const { productId } = useParams();
  const definition = useDefinition("ProductForList");
  const texts = {
    title: "Products",
    add: "Add product",
  };

  const { loading, values: elements } = useSelector(productsState);

  useEffect(() => {
    dispatch(getAll());
    return () => dispatch(disconnect());
  }, [dispatch]);

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

import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { balancesState, disconnect, getAll } from "./slice";

export function BalanceList() {
  const dispatch = useDispatch();
  const { balanceId } = useParams();
  const definition = useDefinition("BalanceForList");
  const texts = {
    title: "Balances",
    add: "Add balance",
  };
  const { loading, values: elements } = useSelector(balancesState);

  useEffect(() => {
    dispatch(getAll());
    return () => dispatch(disconnect());
  }, [dispatch]);

  return (
    <PageList definition={definition} loading={loading} elements={elements} selectedId={balanceId} texts={texts} />
  );
}

import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { useSorting } from "../helpers/sorting";
import PageList from "../Generic/PageList";

export function BalanceList() {
  const { balanceId } = useParams();
  const definition = useDefinition("BalanceForList");
  const texts = {
    title: "Balances",
    add: "Add balance",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={balanceId}
      texts={texts}
      newRole="time"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}

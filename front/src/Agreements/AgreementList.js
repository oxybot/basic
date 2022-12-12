import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { useSorting } from "../helpers/sorting";
import PageList from "../Generic/PageList";

export function AgreementList() {
  const { agreementId } = useParams();
  const definition = useDefinition("AgreementForList");
  const texts = {
    title: "Agreements",
    add: "Add agreement",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={agreementId}
      texts={texts}
      newRole="client"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}

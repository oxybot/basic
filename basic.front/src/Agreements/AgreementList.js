import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageList from "../Generic/PageList";

export default function Agreements() {
  const { agreementId } = useParams();
  const definition = useDefinition("AgreementForList");
  const [loading, elements] = useApiFetch("Agreements", { method: "GET" }, []);
  const texts = {
    title: "Agreements",
    add: "Add agreement",
  };
  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={agreementId}
      texts={texts}
    />
  );
}

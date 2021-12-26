import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

export function AgreementNew() {
  const definition = useDefinition("AgreementForEdit");

  const texts = {
    title: "Agreements",
    subTitle: "Add a new Agreement",
  };

  return (
    <PageNew definition={definition} baseApiUrl="Agreements" texts={texts} />
  );
}

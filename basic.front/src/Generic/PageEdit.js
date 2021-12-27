import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { apiUrl, retries } from "../api";
import EntityForm from "./EntityForm";

const defaultTransform = (e) => e;
const defaultOnUpdate = () => {};

export default function PageEdit({
  definition,
  baseApiUrl,
  entityId,
  full = false,
  texts,
  onUpdate = defaultOnUpdate,
  transform = defaultTransform,
}) {
  const navigate = useNavigate();
  const [entity, setEntity] = useState({});
  const [validated, setValidated] = useState(false);
  texts["form-action"] = "Update";

  useEffect(() => {
    retries(() => fetch(apiUrl(baseApiUrl, entityId), { method: "GET" }))
      .then((response) => response.json())
      .then((response) => {
        setEntity(transform(response));
      })
      .catch((err) => console.log(err));
  }, [baseApiUrl, entityId, transform]);

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setEntity({ ...entity, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    setValidated(true);
    fetch(apiUrl(baseApiUrl, entityId), {
      method: "PUT",
      headers: {
        "content-type": "application/json",
        accept: "application/json",
      },
      body: JSON.stringify(entity),
    })
      .then((response) => {
        if (response.ok) {
          navigate("./..");
          onUpdate();
        } else {
          throw new Error(response);
        }
      })
      .catch((err) => {
        console.error(err);
        alert(err);
      });
  };

  return (
    <EntityForm
      definition={definition}
      entity={entity}
      full={full}
      texts={texts}
      handleChange={handleChange}
      handleSubmit={handleSubmit}
      validated={validated}
    />
  );
}

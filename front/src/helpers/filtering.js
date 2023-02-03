import { useMemo } from "react";
import { useSearchParams } from "react-router-dom";

export function useFiltering() {
  const [searchParams, setSearchParams] = useSearchParams();

  const filters = useMemo(() => {
    if (searchParams && searchParams.getAll("f")) {
      return Object.fromEntries(searchParams.getAll("f").map((e) => [e, true]));
    } else {
      return null;
    }
  }, [searchParams]);

  const setFilters = function (updatedFilters) {
    let updated = new URLSearchParams(searchParams);
    updated.delete("f");
    Object.entries(updatedFilters)
      .filter((p) => p[1] !== false)
      .forEach((p) => {
        updated.append("f", p[0]);
      });

    setSearchParams(updated);
  };

  return [filters, setFilters];
}

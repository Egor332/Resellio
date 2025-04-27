export default function getEnvVariable(env: Record<string, string>, key: string): string {
    const value = env[key];
    if (!value) {
        throw new Error(`Environment variable "${key}" is required but not defined.`);
    }
    return value;
}